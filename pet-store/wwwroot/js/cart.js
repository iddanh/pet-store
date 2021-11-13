function getCartContents() {
    let cart = window.localStorage.getItem('cart');
    if (cart) {
        try {
            return JSON.parse(cart);
        } catch (e) {
            return [];
        }
    }
    return [];
}

function setCartContents(cart) {
    window.localStorage.setItem('cart', JSON.stringify(cart));
}

function addToCart(productId) {
    const cart = getCartContents();

    cart.push(productId);
    setCartContents(cart);

    console.log('Added product to cart', productId);
}

function populateCartDropdown() {
    const cart = getCartContents();

    if (cart.length > 0) {
        $.get(`/api/Cart/${cart.join(',')}`, (data) => {
            $('#cart').empty();

            let totalPrice = 0;
            data.forEach((product) => {
                if (!product) {
                    return;
                }
                $('#cart').append(`
                <a class="cart-item text-decoration-none" href="/Products/Details/${product.id}">
                    <div class="cart-img-container">
                        <img src="${product.image}" />
                    </div>
                    <div>
                        <div>${product.name}</div>
                        <div class="price">${product.price}$</div>
                    </div>
                    <button class="float-right remove-from-cart" data-product-id="${product.id}">&times;</button>
                </div>
            `);
                totalPrice += product.price;
            });
            $('#cart_total').text(`${Math.round(totalPrice * 100) / 100}$`);
            $('#cart-count').text(data.filter(p => p).length);

            $('.remove-from-cart').click((event) => {
                event.preventDefault();
                event.stopPropagation();

                const productId = $(event.target).attr('data-product-id');
                console.log(productId);

                cart.splice(cart.findIndex(id => id == productId), 1);
                setCartContents(cart);
                populateCartDropdown();
                populateCheckoutPage();
            });
        });
    } else {
        $('#cart').append('<div class="text-center">Cart is empty</div');
        $('#cart-count').text('');
    }
}

function clearCart(event) {
    setCartContents([]);
    populateCartDropdown();
}

function populateCheckoutPage() {
    if ($('#cart-content').length === 0) {
        return;
    }

    const cart = getCartContents();
    if (cart.length > 0) {
        $.get(`/api/Cart/${cart.join(',')}`, (data) => {
            $('#cart-content').empty();
            let productsList = [];
            let totalPrice = 0;
            data.forEach((product) => {
                if (!product) {
                    return;
                }
                $('#cart-content').append(`
                <a class="cart-item text-decoration-none" href="/Products/Details/${product.id}"> 
                    <div class="cart-img-container">
                        <img src="${product.image}" />
                    </div>
                    <div>
                        <div>${product.name}</div>
                        <div class="price">${product.price}$</div>
                    </div>
                    <button class="float-right remove-from-checkout" data-product-id="${product.id}">&times;</button>
                </div>
            `);

                totalPrice += product.price;
                productsList.push(product.id);
            });
            $('#priceinput').val(Math.round(totalPrice * 100) / 100);
            $('#productsinput').val(productsList.join(','));

            $('.remove-from-checkout').click((event) => {
                event.preventDefault();
                event.stopPropagation();

                const productId = $(event.target).attr('data-product-id');
                console.log(productId);

                //there is bug here. it removes the last product each
                cart.splice(cart.findIndex(id => id == productId), 1);
                setCartContents(cart);
                populateCartDropdown();
                populateCheckoutPage();
            });
        });
    } else {
        $('#cart-content').append('<div class="text-center">Cart is empty</div');
        $('#priceinput').val(0);
        $('#productsinput').val('');

    }
}