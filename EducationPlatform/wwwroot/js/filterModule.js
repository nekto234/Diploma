$('#checkboxTest').change(function () {
    if ($(this).is(':checked')) {
        $('#minValTest').attr('disabled', false);
        $('#maxValTest').attr('disabled', false);
    } else {
        $('#minValTest').attr('disabled', true);
        $('#maxValTest').attr('disabled', true);
    }
});


$('#checkboxLab').change(function () {
    if ($(this).is(':checked')) {
        $('#minValLab').attr('disabled', false);
        $('#maxValLab').attr('disabled', false);
    } else {
        $('#minValLab').attr('disabled', true);
        $('#maxValLab').attr('disabled', true);
    }
});



$('#checkboxLab').change();

$('#checkboxTest').change();