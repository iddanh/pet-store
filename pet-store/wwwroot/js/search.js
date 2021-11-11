function search() {
    var minPrice = $('input[name="rangeOne"]');
    var maxPrice = $('input[name="rangeTwo"]');
    var inpSearch = $('#inpSearch');
    var inpCategory = $('#inpCategory');
    var inpCompany = $('#inpCompany');
    var token = $('input[name="__RequestVerificationToken"]').val();

    if (maxPrice.val() < minPrice.val()) {
        var temp = maxPrice;
        maxPrice = minPrice;
        minPrice = temp;
    }
    //var loader = $(this).next();
    $.ajax({
        type: "POST",
        dataType: "json",            
        url: "/Products/Search",
        data: {
            searchString: inpSearch.val(),
            productCategory: inpCategory.val(),
            productCompany: inpCompany.val(),
            minPrice: minPrice.val(),
            maxPrice: maxPrice.val(),
            __RequestVerificationToken: token
        }
    }).done(function (result) {
        var tbody = $('#prodList');
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
    try {
        var searchForm = this.getElementById("searchForm");
        searchForm.addEventListener("submit", search, "capture");
    } catch (e) { };
    try {
        var btnSearch = this.getElementById("btnSearch");
        btnSearch.addEventListener("click", search, "capture");
    } catch (e) { };
});
$("#searchForm").submit(function (e) {
    e.preventDefault();
});

/* Filter
 * -------------------------------------------------- */
var rangeOne = document.querySelector('input[name="rangeOne"]'),
    rangeTwo = document.querySelector('input[name="rangeTwo"]'),
    outputOne = document.querySelector('.outputOne'),
    outputTwo = document.querySelector('.outputTwo'),
    inclRange = document.querySelector('.incl-range'),
    updateView = function () {
        if (this.getAttribute('name') === 'rangeOne') {
            outputOne.innerHTML = this.value + "$";
            outputOne.style.left = this.value / Number.parseInt(this.getAttribute('max')) * 100 + '%';
        } else {
            outputTwo.style.left = this.value / Number.parseInt(this.getAttribute('max')) * 100 + '%';
            outputTwo.innerHTML = this.value + "$";
        }
        if (parseInt(rangeOne.value) > parseInt(rangeTwo.value)) {
            inclRange.style.width = (rangeOne.value - rangeTwo.value) / Number.parseInt(this.getAttribute('max')) * 100 + '%';
            inclRange.style.left = rangeTwo.value / Number.parseInt(this.getAttribute('max')) * 100 + '%';
        } else {
            inclRange.style.width = (rangeTwo.value - rangeOne.value) / Number.parseInt(this.getAttribute('max')) * 100 + '%';
            inclRange.style.left = rangeOne.value / Number.parseInt(this.getAttribute('max')) * 100 + '%';
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