$(function () {
    $('.btnSearch').click(function () {
        var inpSearch = $('#inpSearch');
        var inpCategory = $('#inpCategory');
        var inpCompany = $('#inpCompany');
        var token = $('input[name="__RequestVerificationToken"]').val();
        //var loader = $(this).next();
        $.ajax({
            type: "POST",
            dataType: "json",            
            url: "/Products/Search",
            data: {
                searchString: inpSearch.val(),
                productCategory: inpCategory.val(),
                productCompany: inpCompany.val(),
                __RequestVerificationToken: token
            }
        }).done(function (result) {
            window.HTMLAnchorElement="/Product"
            var tbody = $('tbody');
            var template = $('#template').html();
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
            //loader.addClass('d-none');
        });
    });
});