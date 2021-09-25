function getCartContents() {
    let cart = window.sessionStorage.getItem('cart');
    if (cart) {
        try {
            cart = JSON.parse(cart);
        } catch (e) {
            cart = [];
        }
    } else {
        cart = [];
    }
}

function addToCart(product) {
    const cart = getCartContents();

    cart.push(product);
    window.sessionStorage.setItem('cart', JSON.stringify(cart));

    console.log('Added product to cart', product);
    $('#add_cart').text('Added').removeClass('btn-primary').addClass('btn-success');
}

function populateCartDropdown() {
    const cart = getCartContents();

    cart.forEach((product) => {
        $('#cart').html('');
        $('#cart').append(`
            <div class="cart-item">
                <div class="cart-img-container">
                    <img src="${product.image}" />
                </div>
                <div>
                    <div>${product.name}</div>
                    <div class="price">${product.price}$</div>
                </div>
            </div>
        `);
    });
}