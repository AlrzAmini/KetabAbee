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


function LoadAddAnswerModalBody(commentId) {
    $.ajax({
        url: "/load-modal-answer",
        type: "get",
        data: {
            commentId: commentId
        },
        beforeSend: function () {
           
        },
        success: function (response) {
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

function LoadContactUsModalBody() {
    $.ajax({
        url: "/load-contactus-modal",
        type: "get",
        data: {
           
        },
        beforeSend: function () {
            
        },
        success: function (response) {
            $("#ContactUsModalContent").html(response);

            $('#ContactUsForm').data('validator', null);
            $.validator.unobtrusive.parse('#ContactUsForm');

            $("#ContactUsModal").modal("show");
        },
        error: function () {
            console.log("Error");
        }
    });
}

function LoadAddBookRequestModalBody(audiobookId) {
    $.ajax({
        url: "/load-modal-request-book",
        type: "get",
        data: {
            audiobookId: audiobookId
        },
        beforeSend: function () {
            
        },
        success: function (response) {
            
            $("#RequestModalContent").html(response);

            $('#RequestForm').data('validator', null);
            $.validator.unobtrusive.parse('#RequestForm');

            $("#RequestModal").modal("show");
        },
        error: function () {
            console.log("Error");
        }
    });
}
function LoadCreateQuestionABookModalBody(audiobookId) {
    $.ajax({
        url: "/load-modal-create-ABook-question",
        type: "get",
        data: {
            audiobookId: audiobookId
        },
        beforeSend: function () {
            
        },
        success: function (response) {
            
            $("#QuestionModalContent").html(response);

            $('#CreateQuestionForm').data('validator', null);
            $.validator.unobtrusive.parse('#CreateQuestionForm');

            $("#QuestionModal").modal("show");
        },
        error: function () {
            console.log("Error");
        }
    });
}
function LoadCreateAnswerABookModalBody(questionId) {
    $.ajax({
        url: "/load-modal-create-ABook-answer",
        type: "get",
        data: {
            questionId: questionId
        },
        beforeSend: function () {
            
        },
        success: function (response) {
            $("#QAnswerModalContent").html(response);

            $('#CreateAnswerForm').data('validator', null);
            $.validator.unobtrusive.parse('#CreateAnswerForm');

            $("#QAnswerModal").modal("show");
        },
        error: function () {
            console.log("Error");
        }
    });
}

