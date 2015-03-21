$(function (e) {
    AddRecipeStep();
    function AddRecipeStep(e) {
        $('.recipe-steps').append(
            '<div class="panel panel-primary recipe-step"> \
                <div class="panel-heading recipe-step-head"> \
                    <div class="pull-right"> \
                        <a href="javascript:void(0)" class="btn btn-xs btn-default recipe-step-delete">Удалить</a> \
                    </div> \
                    <div class="recipe-step-caption">Шаг ' + ($('.recipe-step-head').length + 1) + '</div>\
                </div> \
                <div class="panel-body"> \
                    <div class="form-group"> \
                        <div class="control-label col-md-2">Описание шага</div> \
                        <div class="col-md-10"> \
                            <textarea cols="35" class="form-control recipe-step-discription valid" id="Discription" name="RecipeStep[' + $('.recipe-step-discription').length + '].Discription" rows="4"></textarea> \
                        </div> \
                    </div> \
                    <div class="form-group"> \
                        <div class="control-label col-md-2">Время в минутах</div> \
                        <div class="col-md-10"> \
                            <input type="number" class="recipe-step-time" name="RecipeStep[' + $('.recipe-step-time').length + '].Time"> \
                        </div> \
                    </div> \
                    <div class="form-group"> \
                        <div class="control-label col-md-2">Изображение</div> \
                        <div class="col-md-10"> \
                            <input type="file" id="stepPicture" name="stepPictures" accept="image/*" /> \
                        </div> \
                    </div> \
                </div> \
            </div>'
        );
        $('.recipe-step-delete').click(DeleteRecipeStep);
    }


    $('.add-step').click(AddRecipeStep);

    function DeleteRecipeStep(e) {
        $(this).parents('.recipe-step').remove();

        $('.recipe-step-caption').each(function (i, element) {
            $(element).text('Шаг ' + (i + 1));
        })

        $('.recipe-step-discription').each(function (i, element) {
            $(element).attr('name', 'RecipeStep[' + i + '].Discription');
        });

        $('.recipe-step-time').each(function (i, element) {
            $(element).attr('name', 'RecipeStep[' + i + '].Time');
        });
    }

    $('.recipe-step-delete').click( DeleteRecipeStep );
})