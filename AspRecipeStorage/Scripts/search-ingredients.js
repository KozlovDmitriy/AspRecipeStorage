$(document).ready(function () {
    $('.ingredients-search').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Recipes/AutoCompleteIngredient",
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.Name };
                    }))
                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $('.add-filter-ingredient').click(function (e) {
        var ingredient_name = $('.ingredients-search').val();
        if ( $('.ingredient-name').val() !== ingredient_name ) {
            $.ajax({
                url: "/Recipes/IngredientFilteritem",
                type: "POST",
                dataType: "html",
                data: { ingredientName: ingredient_name },
                success: function (data) {
                    if (data !== null && data !== "") {
                        $('.ingridients').append('<li class="list-group-item clearfix ingridient-item">' + data + '</li>');
                        $('.ingredient-delete').click(DeleteIngredient);
                    }
                }
            })
        }
    });

    function DeleteIngredient(e) {
        $(this).parents('.ingridient-item').remove();
    }
});
