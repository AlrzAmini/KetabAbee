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

function ShowSwal(title, text, icon) {
    swal.fire({
        title: title,
        text: text,
        icon: icon,
        confirmButtonText: 'باشه'
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
$("#go--search").on("click", function () {
    swal.fire({
        title: '',
        text: "گوگل کنید :)))",
        icon: 'success',
        confirmButtonText: 'الله اکبر'
    });
});
$("#compare-info").on("click", function () {
    swal.fire({
        title: '',
        text: "حداکثر تعداد مقایسه محصول برای یک مقایسه میتواند چهار محصول باشد و اگر بیش از چهار محصول اضافه کنید مقایسه جدیدی برای شما ایجاد می شود میتوانید از بالای صفحه لیست مقایسه هایی که انجام داده اید را ببینید",
        icon: 'info',
        confirmButtonText: 'بله'
    });
});

function StartLoading(element = 'body') {
    $(element).waitMe({
        effect: 'bounce',
        text: 'لطفا صبر کنید',
        bg: 'rgba(255, 255, 255, 0.7)',
        color: '#000'
    });
}

function CloseLoading(element = 'body') {
    $(element).waitMe('hide');
}

function LoadAddAnswerModalBody(commentId) {
    $.ajax({
        url: "/load-modal-answer",
        type: "get",
        data: {
            commentId: commentId
        },
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            CloseLoading();
            $("#AnswerModalContent").html(response);

            $('#AnswerForm').data('validator', null);
            $.validator.unobtrusive.parse('#AnswerForm');

            $("#AnswerModal").modal("show");
        },
        error: function () {
            CloseLoading();
            console.log("Error");
        }
    });
}

