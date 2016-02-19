$(document).ready(function () {
    $("#Save").click(function () {

        var Submission = new Object();
        Submission.Username = $('#name').val();
        person.surname = $('#surname').val();

        $.ajax({
            url: '/api/submission',
            type: 'POST',
            dataType: 'json',
            data: person,
            success: function (data, textStatus, xhr) {
                console.log(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    });
});