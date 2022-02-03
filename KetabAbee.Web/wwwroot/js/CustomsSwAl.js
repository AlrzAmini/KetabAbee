/*====== SweetAlert ======*/
HAMTA.SweetAlert = function () {
    $('.link-send-order').on('click', function () {
        Swal.fire({
            text: "پس از ثبت سفارش محصولات شما بین 4 تا 6 روز آینده به دستتان می رسد",
            icon: 'success',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'بله'
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