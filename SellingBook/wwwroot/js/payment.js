let total = 0;
let discountValue = 0; // Giá trị giảm giá

function toggleAllItems() {
    const isChecked = document.getElementById('selectAll').checked;
    document.querySelectorAll('.cartItemCheckbox').forEach(checkbox => {
        checkbox.checked = isChecked;
    });
    updateTotal();
}

function updateTotal() {
    let localTotal = 0;

    document.querySelectorAll('.cartItemCheckbox:checked').forEach(checkbox => {
        let row = checkbox.closest('tr');
        let itemPrice = parseInt(row.cells[4].innerText.replace(/\D/g, ''), 10);
        localTotal += itemPrice;
    });

    total = localTotal - discountValue; // Trừ giá trị giảm giá

    if (total < 0) total = 0; // Không để tổng tiền âm

    document.getElementById('selectedTotalPrice').innerHTML = total.toLocaleString() + " <u>đ</u>";
}

async function applyDiscount() {
    let discountCode = document.getElementById('discountCode').value;

    const response = await fetch('/Customer/Coupon/ValidateCode', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(discountCode)
    });

    if (response.ok) {
        const data = await response.json();
        discountValue = data.discountValue; // Cập nhật giá trị giảm giá
        document.getElementById('discountMessage').innerHTML = `Áp dụng thành công! Giảm ${discountValue.toLocaleString()} đ`;
        document.getElementById('discountMessage').style.color = 'green';
    } else {
        discountValue = 0; // Nếu mã không hợp lệ, không giảm giá
        document.getElementById('discountMessage').innerHTML = `Mã giảm giá không hợp lệ!`;
        document.getElementById('discountMessage').style.color = 'red';
    }

    updateTotal(); // Cập nhật tổng tiền sau khi áp dụng giảm giá
}

async function processPayment(name) {
    let url;
    let payload;
    let selectedItems = [];
    let orderDescription = [];

    document.querySelectorAll('.cartItemCheckbox:checked').forEach(checkbox => {
        let row = checkbox.closest('tr');
        let cartItemId = checkbox.value;
        let productName = row.cells[1].innerText.trim(); // Lấy tên sản phẩm
        let price = parseInt(row.cells[2].innerText.replace(/\D/g, ''), 10); // Lấy giá
        let quantity = row.cells[3].innerText.trim(); // Lấy số lượng
        selectedItems.push({
            CartItemId: cartItemId,
            ProductId: cartItemId,
            CartItemQuantity: quantity,
            CartItemPrice: price
        });
        orderDescription.push(`${productName} x${quantity}`);
    });

    if (selectedItems.length === 0) {
        alert("Vui lòng chọn ít nhất một sản phẩm để thanh toán.");
        return;
    }

    let paymentMethodElement = document.querySelector('input[name="paymentMethod"]:checked');
    if (!paymentMethodElement) {
        alert("Vui lòng chọn phương thức thanh toán.");
        return;
    }

    let paymentMethod = paymentMethodElement.id;

    if (paymentMethod === 'vnpay') {
        url = '/Checkout/CreatePaymentUrlVnPay';
        payload = {
            Amount: total,
            OrderDescription: orderDescription.join("\n"), // Xuống hàng giữa mỗi sản phẩm
            OrderType: "other",
            Name: name
        };
    } else {
        alert("Hiện tại chỉ hỗ trợ thanh toán qua VNPay!");
        return;
    }

    if (!url) {
        console.error("Không tìm thấy URL thanh toán!");
        return;
    }

    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    });

    if (!response.ok) {
        console.error("Không thể lấy URL thanh toán.");
        return;
    } else {
        const data = await response.json();
        window.location.href = data.paymentUrl;
        await fetch('/Checkout/PrepareForOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(selectedItems)
        });
    }
}
