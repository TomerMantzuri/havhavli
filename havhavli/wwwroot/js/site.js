$(function () {
    $('.addProduct').click(function () {
        addToCart($(this).attr('Product-id'), 1);
    });
});

function addToCart(id, quantity) {
    $.ajax({
        method: 'post',
        url: '/CartItems/Create',
        data: { 'ProductId': id, 'Quantity': quantity }
    }).done(function (data) {
        console.log(data);
    });
}

$(function () {
    $(".button").on("click", function () {
        var $button = $(this);
        var $parent = $button.parent();
        var oldValue = $parent.find('.input').val();

        if ($button.text() == "+") {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 1) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 1;
            }
        }
        $parent.find('a.add-to-cart').attr('data-quantity', newVal);
        $parent.find('.input').val(newVal);
    });
});

