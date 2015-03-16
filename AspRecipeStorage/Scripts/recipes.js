$(function () {
    $('.dish-type-filter-all').prop('checked', $('.dish-type-filter:not(:checked)').length == 0);
    $('.dish-type-filter').click(function (e) {
        $('.dish-type-filter-all').prop('checked', $('.dish-type-filter:not(:checked)').length == 0);
    });

    $('.dish-type-filter-all').click(function (e) {
        $('.dish-type-filter').prop('checked', this.checked);
    });
})