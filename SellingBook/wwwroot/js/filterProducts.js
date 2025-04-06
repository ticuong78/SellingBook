// filterProducts.js

// Lấy từ khóa từ thuộc tính data của body hoặc một phần tử khác
const keyword = document.body.dataset.keyword || '';

// Hàm để tải danh sách danh mục
function loadCategories() {
    fetch('/api/categories')
        .then(res => res.json())
        .then(categories => {
            const categoryFilter = document.getElementById('categoryFilter');
            categories.forEach(category => {
                const option = document.createElement('option');
                option.value = category.categoryId;
                option.textContent = category.categoryName;
                categoryFilter.appendChild(option);
            });
        })
        .catch(err => {
            console.error('Lỗi khi tải danh mục:', err);
        });
}

// Hàm để tải danh sách sản phẩm theo danh mục
// Xử lý khi người dùng thay đổi danh mục
document.getElementById('categoryFilter').addEventListener('change', function () {
    const categoryId = this.value;
    loadProducts(categoryId);
});

// Hàm để tải danh sách sản phẩm theo danh mục
function loadProducts(categoryId) {
    const url = `/api/products/filter?keyword=${encodeURIComponent(keyword)}${categoryId ? `&categoryId=${categoryId}` : ''}`;
    fetch(url)
        .then(res => res.json())
        .then(products => {
            const productList = document.getElementById('productList');
            if (products.length === 0) {
                productList.innerHTML = '<p>Không tìm thấy sản phẩm nào.</p>';
                return;
            }

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
        })
        .catch(err => {
            console.error('Lỗi khi tải sản phẩm:', err);
            document.getElementById('productList').innerHTML = '<p class="text-danger">Có lỗi xảy ra khi tải sản phẩm.</p>';
        });
}

// Khởi tạo khi trang được tải
document.addEventListener('DOMContentLoaded', function () {
    loadCategories();
    loadProducts(''); // Tải sản phẩm ban đầu (không lọc danh mục)

    // Xử lý khi người dùng thay đổi danh mục
    document.getElementById('categoryFilter').addEventListener('change', function () {
        const categoryId = this.value;
        loadProducts(categoryId);
    });
});