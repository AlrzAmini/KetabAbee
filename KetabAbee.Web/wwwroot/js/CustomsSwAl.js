/*====== SweetAlert ======*/
HAMTA.SweetAlert = function () {
    $('.user-item.cart-list > ul li.cart-items ul .cart-item .remove-item').on('click', function () {
        Swal.fire({
            text: "از سبد خریدتان حذف شود؟",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'بله',
            cancelButtonText: 'خیر'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'حذف شد!',
                    confirmButtonText: 'باشه',
                    icon: 'success'
                })
            }
        })
    });
    $('.product-card .product-card-bottom .btn-add-to-cart').on('click', function (event) {
        event.preventDefault();
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'success',
            title: 'به سبد خریدتان اضافه شد'
        })
    });
    $('.product-card .product-card-actions .add-to-wishlist').on('click', function (event) {
        event.preventDefault();
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'success',
            title: 'به لیست علاقمندی اضافه شد'
        })
    });
    $('.product-card .product-card-actions .add-to-compare').on('click', function (event) {
        event.preventDefault();
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'success',
            title: 'برای مقایسه اضافه شد'
        })
    });
}
    /*====== end SweetAlert ======*/