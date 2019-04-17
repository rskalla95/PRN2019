function updateGroupPermission(id, level, resultField) {
	return new Promise((resolve, reject) => {
		$.ajax({
			url: "/BlogAPI/AdminApi/GroupPermission/" + id.toString() + "/" + level.toString(),
			type: "POST",
			success: data => {
				resultField.text(data);
			},
			error: error => {
				resultField.text("There was an error saving. Please reload the page.");
				$("select").prop('disabled', 'disabled');
				console.error("Couldn't update permission.");
				reject();
			}
		});
	});
}

$(document).ready(function () {
	$(".manage-blogger-groups").on("change", "select", function () {
		var permissionId = $(this).parent().data("permissionId");
		var resultField = $(this).parent().parent().find(".permission-description");
		var level = $(this).val();
		updateGroupPermission(permissionId, level, resultField);
	});
});