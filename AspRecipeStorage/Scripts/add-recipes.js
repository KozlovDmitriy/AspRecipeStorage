var addingIngredientStep = null;
var addingInstrumentStep = null;

function UpdateStepsAttributes() {
    $('.recipe-step').each(function (i, element) {
        $('.recipe-step-caption', element).text('Шаг ' + (i + 1));
        $('.child-recipe', element).attr('name', 'RecipeSteps[' + i + '].ChildRecipeId');
        $('.recipe-step-discription', element).attr('name', 'RecipeSteps[' + i + '].Discription');
        $('.recipe-step-time', element).attr('name', 'RecipeSteps[' + i + '].Time');
        $('.recipe-step-id', element).attr('name', 'RecipeSteps[' + i + '].Id');
        $('.recipe-step-images', element).attr('name', 'stepPictures[' + i + ']');
        $('.ingredient', element).each(function (j, ingredient) {
            $('.ingredient-name', ingredient).attr('name', 'RecipeSteps[' + i + '].Ingredients[' + j + '].IngredientType.Name');
            $('.ingredient-measure', ingredient).attr('name', 'RecipeSteps[' + i + '].Ingredients[' + j + '].MeasureTypeId');
            $('.ingredient-amount', ingredient).attr('name', 'RecipeSteps[' + i + '].Ingredients[' + j + '].Amount');
            $('.ingredient-id', ingredient).attr('name', 'RecipeSteps[' + i + '].Ingredients[' + j + '].Id');
        });
        $('.instrument', element).each(function (j, instrument) {
            $('.instrument-name', instrument).attr('name', 'RecipeSteps[' + i + '].StepInstruments[' + j + '].Instrument.Name');
            $('.instrument-amount', instrument).attr('name', 'RecipeSteps[' + i + '].StepInstruments[' + j + '].InstrumentCount');
            $('.instrument-id', instrument).attr('name', 'RecipeSteps[' + i + '].StepInstruments[' + j + '].Id');
        });
    });
}
function OnAddRecipeStepHierarchy() {
    $('.recipe-step-delete').click(DeleteRecipeStep);
    UpdateStepsAttributes();
}

function OnAddRecipeStep() {
    $('.recipe-step-delete').click(DeleteRecipeStep);
    $('.add-ingredient').click(SaveAddingIngredientStep);
    $('.add-instrument').click(SaveAddingInstrumentStep);
    UpdateStepsAttributes();
}

function OnAddInstrument(data) {
    $('.instruments', $(addingInstrumentStep)).append(data);
    $('.instrument-delete').click(DeleteInstrument);
    addingInstrumentStep = null;
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
    addingInstrumentStep = $(this).parents('.recipe-step');
}

$(document).ready(function () {
    $('.add-instrument').click(SaveAddingInstrumentStep);
    $('.add-ingredient').click(SaveAddingIngredientStep);
    $('.recipe-step-delete').click(DeleteRecipeStep);
    $('.ingredient-delete').click(DeleteIngredient);
    $('.instrument-delete').click(DeleteInstrument);
    UpdateStepsAttributes();
})