$(document).ready(function () {
	tinymce.init({
		selector: "#post-body-edit",
		plugins: 'advlist autolink link image media lists charmap print preview hr anchor pagebreak spellchecker searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking save table contextmenu directionality emoticons template paste textcolor',
		toolbar: 'indent outdent | bold italic underline forecolor backcolor | alignleft aligncenter alignright | bullist numlist | link image media | file code',
		toolbar_items_size: 'small'
	});

	var blogID = $(".post-tags-input").data("blogId");
	$.get("/BlogAPI/Tags/Tags/" + blogID.toString(),
		function (data) {
			if (data == "") {
				data = null;
			}

			// if there is only one tag put it in an array so that tagSuggest knows what to do with it.
			if (data.indexOf(',') < 0) {
				data = [data];
			}

			var TagBox = $('.post-tags-input').tagSuggest({
				data: data,
				sortOrder: 'name',
				maxDropHeight: 200,
				name: 'Tags',
				emptyText: "Tags",
				useTabKey: true,
				value: $(".post-tags-input").val().split(',')
			});
			console.log(data);
		});

	$("form").submit(function () {
		$("input[name='Tags']").val(eval($("input[name='Tags']").val()));
	});
});

function UpdateFileName(e) {
	if (e.files[0].type == "image/jpeg" || e.files[0].type == "image/png") {
		$("#FileText").css({ "color": "inherit", "font-weight": "normal" });

		if (e.files && e.files[0]) {
			$("#FileText").html(e.files[0].name);

			var reader = new FileReader();
			reader.onload = function (event) {

				var canvas = document.getElementById('canvas'),
					ctx = canvas.getContext('2d');

				var img = new Image();

				img.onload = function () {
					canvas.width = canvas.height * (img.width / img.height);

					var oc = document.createElement('canvas'),
						octx = oc.getContext('2d');

					oc.width = img.width;
					oc.height = img.height;

					octx.drawImage(img, 0, 0, oc.width, oc.height);
					octx.drawImage(oc, 0, 0, oc.width, oc.height);

					ctx.drawImage(oc, 0, 0, oc.width, oc.height, 0, 0, canvas.width, canvas.height);

					var data = canvas.toDataURL(e.files[0].type);

					$("#preview-image").attr('style', "background-image: url(" + data + ")");
					$("#preview-image").show();

				}
				img.src = event.target.result;
			}
			reader.readAsDataURL(e.files[0]);
		}
	}
	else {
		$("#preview-image").hide();
		$("#FileText").html("Must be JPEG or PNG");
		$("#FileText").css({ "color": "red", "font-weight": "bold" });
	}
}