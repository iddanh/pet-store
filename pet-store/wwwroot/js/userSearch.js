$(function () {
    $('.btnSearch').click(function () {
        var name = $('input[name="name"]');
        var email = $('input[name="email"]');
        var type = $('select[name="type"]');
        var company = $('select[name="company"]');
        var token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/Users/UserSearch",
            data: {
                name: name.val(),
                email: email.val(),
                company: company.val(),
                type: type.val(),
                __RequestVerificationToken: token
            }
        }).done(function (result) {
            var tbody = $('tbody');
            var template = $('#template').html();
            tbody.html('');
            $.each(result, function (key, value) {
                var temp = template;
                $.each(value, function (key, value) {
                    temp = temp.replaceAll('${' + key + '}', value);
                });
                tbody.append(temp);
            });
        });
    });
});