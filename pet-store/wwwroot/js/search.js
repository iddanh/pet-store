function search() {
    var minPrice = $('#minPrice');
    var maxPrice = $('#maxPrice');
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
        var tbody = $('.row-cols-4');
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
};

$(function () {
    var searchForm = this.getElementById("searchForm");
    searchForm.addEventListener("submit", search, "capture");
    var btnSearch = this.getElementById("btnSearch");
    btnSearch.addEventListener("click", search, "capture");
});
$("#searchForm").submit(function (e) {
    e.preventDefault();
});

var rangeOne = document.querySelector('input[name="rangeOne"]'),
    rangeTwo = document.querySelector('input[name="rangeTwo"]'),
    outputOne = document.querySelector('.outputOne'),
    outputTwo = document.querySelector('.outputTwo'),
    inclRange = document.querySelector('.incl-range'),
    updateView = function () {
        if (this.getAttribute('name') === 'rangeOne') {
            outputOne.innerHTML = this.value + "$";
            outputOne.style.left = this.value / this.getAttribute('max') * 100 + '%';
        } else {
            outputTwo.style.left = this.value / this.getAttribute('max') * 100 + '%';
            outputTwo.innerHTML = this.value + "$";
        }
        if (parseInt(rangeOne.value) > parseInt(rangeTwo.value)) {
            inclRange.style.width = (rangeOne.value - rangeTwo.value) / this.getAttribute('max') * 100 + '%';
            inclRange.style.left = rangeTwo.value / this.getAttribute('max') * 100 + '%';
        } else {
            inclRange.style.width = (rangeTwo.value - rangeOne.value) / this.getAttribute('max') * 100 + '%';
            inclRange.style.left = rangeOne.value / this.getAttribute('max') * 100 + '%';
        }
    };

document.addEventListener('DOMContentLoaded', function () {
    updateView.call(rangeOne);
    updateView.call(rangeTwo);
    $('input[type="range"]').on('mouseup', function () {
        this.blur();
    }).on('mousedown input', function () {
        updateView.call(this);
    });
});