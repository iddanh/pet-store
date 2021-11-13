$(function () {
    var categoryId = $('#category').val();
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "/Products/GetBreadCrumbs",
        data: {
            categoryId: categoryId
        }
    }).done(function (result) {
        var tbody = $('.breadcrumb');
        var template = $('#breadcrumbs').html();
        var current = $('#current');
        tbody.html('');
        $.each(result, function (key, value) {
            var temp = template;
            $.each(value, function (key, value) {
                temp = temp.replaceAll('${' + key + '}', value);
                try {
                    $.each(value, function (subkey, subvalue) {
                        if (subvalue != null)
                            temp = temp.replaceAll('${' + key + "." + subkey + '}', subvalue);
                    });
                }
                catch (e) { }
            });
            tbody.append(temp);
        });
        tbody.append(current);
    });
});