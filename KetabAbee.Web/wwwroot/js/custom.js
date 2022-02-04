function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 9000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}
$("#Send-Order-Guid").on("click", function () {
    swal.fire({
        title: '',
        text: 'سفارش شما پس از ثبت بین 4 تا 6 روز کاری به دست شما می رسد',
        icon: 'info',
        confirmButtonText: 'باشه'
    });
});
$("#Pay-Order-Way").on("click", function () {
    swal.fire({
        title: '',
        text: 'برای پرداخت هزینه خرید خود میبایست کیف پول خود را در پروفایل کاربری به اندازه ای که نیاز دارید شارژ کرده و اقدام به خرید کنید',
        icon: 'info',
        confirmButtonText: 'باشه'
    });
});
$("#Return-Product-Way").on("click", function () {
    swal.fire({
        title: '',
        text: 'برای بازگردانی کالا کافیست شماره فاکتور خود و علت را برای ما از طریق پروفایل کاربری خریدار تیکت کنید',
        icon: 'info',
        confirmButtonText: 'باشه'
    });
});
$("#Privecy--User").on("click", function () {
    swal.fire({
        title: '',
        text: "کاربر عزیز اطلاعتی که شما در اختیار وبسایت ما قرار میدهید به هیچ عنوان در دسترس شخصی قرار نخواهد گرفت",
        icon: 'success',
        confirmButtonText: 'باشه'
    });
});

