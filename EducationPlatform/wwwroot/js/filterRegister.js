$('#checkboxAgree').change(function () {
    if ($(this).is(':checked')) {
        $('#register').attr('disabled', false);
    } else {
        $('#register').attr('disabled', true);
    }
});