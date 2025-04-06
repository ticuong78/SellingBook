// sortProducts.js

function sortProducts(sort) {
    const productList = document.getElementById('productList');
    if (!productList) {
        console.error('Không tìm thấy phần tử productList trong DOM');
        return;
    }

    productList.innerHTML = '<p>Đang tải sản phẩm...</p>';

    const keyword = document.body.dataset.keyword || '';
    const categoryId = document.getElementById('categoryFilter')?.value || '';
    const page = 1;
    const pageSize = 9;

    const url = `/api/products/sort?keyword=${encodeURIComponent(keyword)}${categoryId ? `&categoryId=${categoryId}` : ''}&sort=${sort}&page=${page}&pageSize=${pageSize}`;
    console.log('URL API (Sort):', url);

    fetch(url)
        .then(res => {
            console.log('Phản hồi từ API /api/products/sort:', res);
            if (!res.ok) {
                throw new Error(`HTTP error! Status: ${res.status}`);
            }
            return res.json();
        })
        .then(data => {
            console.log('Dữ liệu trả về (Sort):', data);
            const products = data.products || [];

            if (!products || products.length === 0) {
                productList.innerHTML = '<p>Không tìm thấy sản phẩm nào.</p>';
            } else {
                productList.innerHTML = products.map(product => `
                    <div class="col-md-4 mb-4">
                        <div class="card h-100">
                            <img src="${product.imageUrl}" class="card-img-top" alt="${product.productName}" style="height: 200px; object-fit: cover;" />
                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title">${product.productName}</h5>
                                <p class="card-text text-muted">Danh mục: ${product.categoryName}</p>
                                <p class="card-text text-danger fw-bold">${product.productPrice.toLocaleString()} <u>đ</u></p>
                                <a href="/Customer/Product/Display/${product.productId}" class="btn btn-primary mt-auto">Xem chi tiết</a>
                            </div>
                        </div>
                    </div>
                `).join('');
            }
        })
        .catch(err => {
            console.error('Lỗi khi tải sản phẩm (Sort):', err);
            productList.innerHTML = '<p class="text-danger">Có lỗi xảy ra khi tải sản phẩm.</p>';
        });
}

document.addEventListener('DOMContentLoaded', function () {
    const sortFilter = document.getElementById('sortFilter');
    if (sortFilter) {
        sortFilter.addEventListener('change', function () {
            const sort = this.value;
            console.log('Kiểu sắp xếp được chọn:', sort);
            sortProducts(sort);
        });
    } else {
        console.error('Không tìm thấy phần tử sortFilter để gắn sự kiện change');
    }
});