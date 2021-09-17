$(() => {
    const options = {
        cloudName: 'iddanh',
        apiKey: '184519112571914',
        uploadPreset: 'unsigned',
        googleApiKey: 'AIzaSyBkN-oAACs5jkJLiENYPQs_jTUWIyNXHfo'
    };

    const processResults = (error, result) => {
        if (!error && result && result.event === 'success') {
            $('#image_input').val(result.info.secure_url);
            $('#image_preview').attr('src', result.info.thumbnail_url);
        }
    };

    const myWidget = window.cloudinary.createUploadWidget(options, processResults);
    $('#upload_widget').click(() => myWidget.open());
});