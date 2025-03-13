let total = 0;

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

    total = localTotal;
    document.getElementById('selectedTotalPrice').innerHTML = total.toLocaleString() + " <u>đ</u>";
}

async function processPayment(name) {
    let url;
    let payload;
    let selectedItems = [];
    document.querySelectorAll('.cartItemCheckbox:checked').forEach(checkbox => {
        selectedItems.push(checkbox.value);
    });

    if (selectedItems.length === 0) {
        alert("Vui lòng chọn ít nhất một sản phẩm để thanh toán.");
        return;
    }

    let paymentMethod = document.querySelector('input[name="paymentMethod"]:checked').id;

    if (paymentMethod === 'vnpay') {
        url = '/Checkout/CreatePaymentUrlVnPay';
        payload = {
            Amount: total,
            OrderDescription: "Thanh toán VNPay",
            OrderType: "other",
            Name: name
        }
    }

    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    })

    if (!response.ok) {
        console.error("Failed to get payment URL");
        return;
    }

    const data = await response.json();
    window.location.href = data.paymentUrl; // Redirect user to VNPay
}