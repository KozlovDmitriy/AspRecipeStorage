var addingIngredientStep = null;
var addingInstrumeentStep = null;

function UpdateStepsAttributes() {
    $('.recipe-step').each(function (i, element) {
        $('.recipe-step-caption', element).text('Шаг ' + (i + 1));
        $('.recipe-step-discription', element).attr('name', 'RecipeStep[' + i + '].Discription');
        $('.recipe-step-time', element).attr('name', 'RecipeStep[' + i + '].Time');
        $('.recipe-step-id', element).attr('name', 'RecipeStep[' + i + '].Id');
        $('.recipe-step-images', element).attr('name', 'stepPictures[' + i + ']');
        $('.ingredient', element).each(function (j, ingredient) {
            $('.ingredient-name', ingredient).attr('name', 'RecipeStep[' + i + '].Ingredients[' + j + '].IngredientType.Name');
            $('.ingredient-measure', ingredient).attr('name', 'RecipeStep[' + i + '].Ingredients[' + j + '].MeasureTypeId');
            $('.ingredient-amount', ingredient).attr('name', 'RecipeStep[' + i + '].Ingredients[' + j + '].Amount');
            $('.ingredient-id', ingredient).attr('name', 'RecipeStep[' + i + '].Ingredients[' + j + '].Id');
        });
        $('.ingstrument', element).each(function (j, ingstrument) {
            $('.ingstrument-name', ingstrument).attr('name', 'RecipeStep[' + i + '].StepInstruments[' + j + '].Instrument.Name');
            $('.ingstrument-amount', ingstrument).attr('name', 'RecipeStep[' + i + '].StepInstruments[' + j + '].InstrumentCount');
            $('.ingstrument-id', ingstrument).attr('name', 'RecipeStep[' + i + '].StepInstruments[' + j + '].Id');
        });
    });
}

function OnAddRecipeStep() {
    $('.recipe-step-delete').click(DeleteRecipeStep);
    $('.add-ingredient').click(SaveAddingIngredientStep);
    UpdateStepsAttributes();
}

function OnAddInstrument(data) {
    $('.instruments', $(addingInstrumeentStep)).append(data);
    $('.instrument-delete').click(DeleteIngredient);
    addingInstrumeentStep = null;
    $('.instrument-name').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Recipes/AutoCompleteInstrument",
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
    UpdateStepsAttributes();
}

function OnAddIngredient(data) {
    $('.ingredients', $(addingIngredientStep)).append(data);
    $('.ingredient-delete').click(DeleteIngredient);
    addingIngredientStep = null;
    UpdateStepsAttributes();
}

function DeleteInstrument(e) {
    $(this).parents('.instrument').remove();
    UpdateStepsAttributes();
}

function DeleteIngredient(e) {
    $(this).parents('.ingredient').remove();
    UpdateStepsAttributes();
}

function DeleteRecipeStep(e) {
    $(this).parents('.recipe-step').remove();
    UpdateStepsAttributes();
}

function SaveAddingIngredientStep(e) {
    addingIngredientStep = $(this).parents('.recipe-step');
}

function SaveAddingInstrumentStep(e) {
    addingInstrumeentStep = $(this).parents('.recipe-step');
}

$(document).ready(function () {
    $('.add-instrument').click(SaveAddingInstrumentStep);
    $('.add-ingredient').click(SaveAddingIngredientStep);
    $('.recipe-step-delete').click(DeleteRecipeStep);
    $('.ingredient-delete').click(DeleteIngredient);
    $('.instrument-delete').click(DeleteIngredient);
    UpdateStepsAttributes();
})