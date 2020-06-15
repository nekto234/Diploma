using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationPlatform.Hubs;
using EducationPlatform.Models.Entities;
using EducationPlatform.Models.EntityModels;
using EducationPlatform.Services;
using EducationPlatform.Services.Interfaces;
using EducationPlatform.Services.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EducationPlatform
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddDbContext<EducationPlatformContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                //.AddMicrosoftAccount(microsoftOptions => { ... })
                .AddGoogle(googleOptions => {
                    var opts = Configuration.GetSection("GoogleAuth");

                    googleOptions.ClientId = opts.GetValue<string>("ClientId");
                    googleOptions.ClientSecret = opts.GetValue<string>("ClientSecret");

                })
                //.AddTwitter(twitterOptions => { ... })
                .AddFacebook(facebookOptions => {
                    var opts = Configuration.GetSection("FacebookAuth");

                    facebookOptions.AppId = opts.GetValue<string>("AppId");
                    facebookOptions.AppSecret = opts.GetValue<string>("AppSecret");
                });

            services.AddSignalR();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<ISubjectsRepository, SubjectsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IStudentsRepository, StudentsRepository>();
            services.AddScoped<IModulesRepository, ModulesRepository>();
            services.AddScoped<ICoursesRepository, CoursesRepository>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            InitializeIdentity(serviceProvider);
        }

        private void InitializeIdentity(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            Task<IdentityResult> roleResultAdmin;
            Task<IdentityResult> roleResultTeacher;
            Task<IdentityResult> roleResultStudent;
            Task<IdentityResult> roleResultBanned;

            var admin = Configuration.GetSection("Admin");
        
            string email = admin.GetValue<string>("email");
            string password = admin.GetValue<string>("password");

            //Check that there is an Administrator role and create if not
            Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Admin");
            hasAdminRole.Wait();

            if (!hasAdminRole.Result)
            {
                roleResultAdmin = roleManager.CreateAsync(new IdentityRole("Admin"));
                roleResultAdmin.Wait();
            }

            Task<bool> hasTeacherRole = roleManager.RoleExistsAsync("Teacher");
            hasTeacherRole.Wait();

            if (!hasTeacherRole.Result)
            {
                roleResultTeacher = roleManager.CreateAsync(new IdentityRole("Teacher"));
                roleResultTeacher.Wait();
            }

            Task<bool> hasStudentRole = roleManager.RoleExistsAsync("Student");
            hasStudentRole.Wait();

            if (!hasAdminRole.Result)
            {
                roleResultStudent = roleManager.CreateAsync(new IdentityRole("Student"));
                roleResultStudent.Wait();
            }

            Task<bool> hasBannedRole = roleManager.RoleExistsAsync("Banned");
            hasBannedRole.Wait();

            if (!hasBannedRole.Result)
            {
                roleResultBanned = roleManager.CreateAsync(new IdentityRole("Banned"));
                roleResultBanned.Wait();
            }

            //Check if the admin user exists and create it if not
            //Add to the Administrator role

            Task<User> testUser = userManager.FindByEmailAsync(email);
            testUser.Wait();

            if (testUser.Result == null)
            {
                User administrator = new User();
                administrator.Email = email;
                administrator.UserName = email;

                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, password);
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Admin");
                    newUserRole.Wait();
                }
            }

        }
    }
}
