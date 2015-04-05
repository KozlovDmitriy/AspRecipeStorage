var addingIngredientStep = null;

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
    });
}

function OnAddRecipeStep() {
    $('.recipe-step-delete').click(DeleteRecipeStep);
    $('.add-ingredient').click(SaveAddingIngredientStep);
    UpdateStepsAttributes();
}

function OnAddIngredient(data) {
    $('.ingredients', $(addingIngredientStep)).append(data);
    $('.ingredient-delete').click(DeleteIngredient);
    addingIngredientStep = null;
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

$(document).ready(function() {
    $('.add-ingredient').click(SaveAddingIngredientStep);
    $('.recipe-step-delete').click(DeleteRecipeStep);
    $('.ingredient-delete').click(DeleteIngredient);
    UpdateStepsAttributes();
})