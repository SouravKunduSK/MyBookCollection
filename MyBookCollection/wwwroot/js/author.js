$(document).ready(function () {
    // Open Add/Edit modal
    $(".add-or-edit-author").on("click", function () {
        const authorId = $(this).data("id") || 0;
        $("#modal-placeholder").load(`/Author/AddOrEditPartial/${authorId}`, function () {
            $("#authorModal").modal("show");
        });
    });

    // Save Author (Add or Edit)
    $("#modal-placeholder").on("click", "#btnSaveAuthor", function () {
        const authorId = $("#authorId").val();
        const authorName = $("#authorName").val();

        const data = { Id: authorId, Name: authorName };

        const url = authorId == 0 ? "/Author/CreateNewAuthor" : "/Author/EditAuthor";

        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response.success) {
                    $("#authorModal").modal("hide");
                    location.reload(); // Reload to see the updated list
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Save error:", error);
                alert("An error occurred while saving the author.");
            }
        });
    });

    // Open Delete modal
    $(".delete-author").on("click", function () {
        const authorId = $(this).data("id");
        $("#modal-placeholder").load(`/Author/DeletePartial/${authorId}`, function () {
            $("#authorModal").modal("show");
        });
    });

    // Delete Author
    $("#modal-placeholder").on("click", "#btnDeleteAuthor", function () {
        const authorId = $(this).data("id");

        $.ajax({
            type: "POST",
            url: `/Author/DeleteConfirmed/${authorId}`,
            success: function (response) {
                if (response.success) {
                    $("#authorModal").modal("hide");
                    location.reload();
                } else {
                    alert("Error: Unable to delete author.");
                }
            },
            error: function (xhr, status, error) {
                console.error("Delete error:", error);
                alert("An error occurred while deleting the author.");
            }
        });
    });
});
