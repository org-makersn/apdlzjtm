
var container, camera, controls, scene, mesh, renderer;

// Initialize and render components.
//init();
//render();

// Add event to button to choose STL model.
// Load the model and add it to the scene.
//var button = document.getElementById("button");
//button.addEventListener("change", function (event) {

//    // Remove previous mesh.
//    scene.remove(mesh);

//    var file = event.target.files[0];
//    var reader = new FileReader();
//    reader.onload = function (event) {
//        var data = event.target.result;
//        var blob = new Blob([data], { 'type': file.type });
//        var url = window.URL.createObjectURL(blob);

//        var loader = "";
//        if (file.name.toLowerCase().indexOf(".js") > -1) {
//            loader = new THREE.JSONLoader();
//        }
//        else if (file.name.toLowerCase().indexOf(".obj") > -1) {
//            loader = new THREE.OBJLoader();
//        }

//            //
//        else if (file.name.toLowerCase().indexOf(".stl") > -1) {
//            loader = new THREE.STLLoader();
//        }
//        //

//        loader.load(url, function (geometry) {
//            var material = new THREE.MeshLambertMaterial({ color: 0xC97B27 }); //color 재질 색상
//            //var material = new THREE.MeshNormalMaterial({ color: 0xC97B27 }); //color 재질 색상

//            mesh = new THREE.Mesh(geometry, material);
//            mesh.position.set(0, 0, 0);
//            scene.add(mesh);
//        });



//    };
//    reader.readAsArrayBuffer(file);
//}, false);

function init(containId, width, height) {

    container = document.getElementById(containId);
    container.style.width = width;
    container.style.height = height;

    camera = new THREE.PerspectiveCamera(35, width / height, 1, 100000);
    camera.position.set(200, 200, 300);

    controls = new THREE.TrackballControls(camera, container);
    controls.rotateSpeed = 0.2;
    controls.noPan = false; // 마우스 우클릭
    controls.staticMoving = false;
    controls.dynamicDampingFactor = 0.5;

    scene = new THREE.Scene();

    //빼면 검정색 됨
    scene.add(new THREE.AmbientLight(0x999999)); //빼면 뒷면 검정(ambient light - 주변 빛)
    var light = new THREE.DirectionalLight(0xffffff);
    light.position.set(1, 1, 1);
    scene.add(light);
    //

    //차승용 불빛 추가
    var pointLight = new THREE.PointLight(0xa1a1a0, 0.5, 0); // Set the color of the light source (white).
    pointLight.position.set(3200, -3900, 3500); // Position the light source at (x, y, z).
    scene.add(pointLight); // Add the light source to the scene.

    var spotLight = new THREE.SpotLight(0xffffff, .7, 0);
    spotLight.position.set(-700, 1000, 1000);
    spotLight.castShadow = false;
    scene.add(this.spotLight);
    //


    renderer = new THREE.WebGLRenderer({
        preserveDrawingBuffer: true,
        antialias: true
    });
    renderer.setSize(width, height);
    renderer.setClearColor(0xffffff, 1); //배경색
    renderer.gammaInput = true;
    renderer.gammaOutput = true;
    container.appendChild(renderer.domElement);
}

function render() {
    if (renderer != null) {
        renderer.render(scene, camera);
        controls.update();
        requestAnimationFrame(render);
    }
}

function viewerReset() {
    container = null;
    camera = null;
    controls = null;
    scene = null;
    mesh = null;
    renderer = null;
}
