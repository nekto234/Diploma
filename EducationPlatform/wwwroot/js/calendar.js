const calendarEl = document.getElementById('calendar');

const calendar = new FullCalendar.Calendar(calendarEl, {
    plugins: ['interaction', 'dayGrid', 'timeGrid', 'list'],
    locale: 'uk',
    header: {
        left: 'prev,next today',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
    },
    defaultDate: '2020-02-08',
    editable: false,
    navLinks: true, // can click day/week names to navigate views
    eventLimit: true, // allow "more" link when too many events
    events: {
        url: GENERAL.calendarURL,
        failure: function () {}
    },
    loading: function (bool) {}
});

calendar.render();


console.log()
$('.fc-button.fc-button-primary').removeClass('fc-button').removeClass('fc-button-primary').addClass('btn btn-inverse-primary btn-sm mx-1');