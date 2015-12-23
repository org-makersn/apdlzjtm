var Thingiview = function (containerId) {

    this.width = window.innerWidth;
    this.height = window.innerHeight;
    this.containerId = containerId;
    this.models = [];
    this.fogColor = 0xcacaca; // 바닥 
    this.scale = 1;
    this.init();

    var mesh = null;
    var timer = null;
    var prevMousePos = [0, 0];
    var plane = null;
    var showPlane = true;
    var object = null;
    var renderer = null;
  
}

Thingiview.prototype.init = function () {
    var _this = this;
    this.container = document.getElementById(this.containerId);
    this.container.style.WebkitTouchCallout = "none";
    this.container.style.WebkitUserSelect = "none";
    this.container.style.KhtmlUserSelect = "none";
    this.container.style.MozUserSelect = "none";
    this.container.style.userSelect = "none";
    this.container.innerHTML = '';
    this.scene = new THREE.Scene();
    this.camera = new THREE.PerspectiveCamera(37.8, this.width / this.height, 1, 100000);
    this.camera.position.z = 300;
    this.camera.position.y = -500;
    this.camera.position.x = -500;
    this.camera.up = new THREE.Vector3(0, 0, 1);
    this.controls = new THREE.NormalControls(this.camera, this.container);
    this.reflectCamera = new THREE.CubeCamera(0.1, 5000, 512);
    this.scene.add(this.reflectCamera);
    var material = new THREE.MeshPhongMaterial({
        color: 0x000000,
        emissive: 0xffffff,
        shading: THREE.SmoothShading,
        fog: false,
        side: THREE.BackSide
    });
    var skybox = new THREE.Mesh(new THREE.CubeGeometry(1000, 1000, 1000), material);
    skybox.name = 'skybox';
    this.scene.add(skybox);
    var groundPlaneMaterial = new THREE.MeshPhongMaterial({
        color: 0x888888,
        wireframe: false,
        envMap: this.reflectCamera.renderTarget
    });
    var x = 1000;
    var y = 1000;
    var division_x = Math.floor(x / 10);
    var division_y = Math.floor(y / 10);
    this.plane = new THREE.Mesh(new THREE.PlaneGeometry(x, y, division_x, division_y), groundPlaneMaterial);
    this.plane.name = 'plane';
    this.plane.receiveShadow = true;
    this.scene.add(this.plane);
    this.wirePlane = new THREE.Mesh(new THREE.PlaneGeometry(x, y, division_x, division_y), new THREE.MeshPhongMaterial({
        emissive: 0xffffff,
        color: 0x000000,
        wireframe: true,
        wireframeLinewidth: 2
    }));
    this.wirePlane.name = 'planewire';
    this.wirePlane.receiveShadow = true;
    this.wirePlane.position.z = this.plane.position.z + .01;
    this.scene.add(this.wirePlane);
    this.scene.fog = new THREE.FogExp2(this.fogColor, 0.007);

    //기존
    this.renderer = new THREE.WebGLRenderer({
        preserveDrawingBuffer: true,
        antialias: true
    });
    this.renderer.setSize(this.width, this.height);
    this.renderer.setClearColor(0xcacaca, 1); // 배경색
    this.container.appendChild(this.renderer.domElement);
    this.initLights();
}

Thingiview.prototype.resize = function (width, height) {
    this.width = width;
    this.height = height;
    this.camera.aspect = width / height;
    this.camera.updateProjectionMatrix();
    this.renderer.setSize(width, height);
}
Thingiview.prototype.initLights = function () {
    this.spotLight = new THREE.SpotLight(0xffffff, .7, 0);
    this.spotLight.position.set(-700, 1000, 1000);
    this.spotLight.castShadow = false;
    this.scene.add(this.spotLight);
    this.pointLights = [];
    pointLight = new THREE.PointLight(0xa1a1a0, 0.5, 0);
    pointLight.position.set(3200, -3900, 3500);
    this.scene.add(pointLight);
    this.pointLights.push(pointLight);
}
Thingiview.prototype.centerCamera = function () {
    var sceneCenter = undefined;
    var sceneObjects = 0;
    var sceneBox = new THREE.Box3();
    this.scene.traverse(function (object) {
        if (object instanceof THREE.Mesh) {
            if (object.name == "skybox" || object.name == "plane" || object.name == "planewire")
                return;
            sceneObjects += 1;
            object.geometry.computeBoundingBox();
            object.geometry.boundingBox.min.applyMatrix4(object.matrixWorld);
            object.geometry.boundingBox.max.applyMatrix4(object.matrixWorld);
            object.geometry.boundingBox.min.x += object.position.x;
            object.geometry.boundingBox.min.y += object.position.y;
            object.geometry.boundingBox.min.z += object.position.z;
            object.geometry.boundingBox.max.x += object.position.x;
            object.geometry.boundingBox.max.y += object.position.y;
            object.geometry.boundingBox.max.z += object.position.z;
            var objectCenter = object.geometry.boundingBox.center();
            objectCenter.z /= 2;
            sceneBox.min.x = Math.min(sceneBox.min.x, object.geometry.boundingBox.min.x);
            sceneBox.min.y = Math.min(sceneBox.min.y, object.geometry.boundingBox.min.y);
            sceneBox.min.z = Math.min(sceneBox.min.z, object.geometry.boundingBox.min.z);
            sceneBox.max.x = Math.max(sceneBox.max.x, object.geometry.boundingBox.max.x);
            sceneBox.max.y = Math.max(sceneBox.max.y, object.geometry.boundingBox.max.y);
            sceneBox.max.z = Math.max(sceneBox.max.z, object.geometry.boundingBox.max.z);

            //볼륨 추가
            //Thingiview.prototype.compute_vol(object.geometry);

            if (sceneCenter === undefined)
                newCenter = objectCenter.clone();
            else {
                var newCenter = new THREE.Vector3();
                newCenter.sub(objectCenter, sceneCenter);
                newCenter.divideScalar(sceneObjects + 1);
                newCenter.add(sceneCenter);
            }
            sceneCenter = newCenter;
        }
    });
    this.controls.desiredCameraTarget = sceneCenter;
    this.controls.desiredCameraTarget.x = this.controls.desiredCameraTarget.y = 0;
    var distanceX = (sceneBox.max.x - sceneBox.min.x) / 2 / Math.tan(this.controls.camera.fov * this.controls.camera.aspect * Math.PI / 360);
    var distanceY = (sceneBox.max.y - sceneBox.min.y) / 2 / Math.tan(this.controls.camera.fov * this.controls.camera.aspect * Math.PI / 360);
    var distanceZ = (sceneBox.max.z - sceneBox.min.z) / 2 / Math.tan(this.controls.camera.fov * Math.PI / 360);
    var distance = Math.max(Math.max(distanceX, distanceY), distanceZ);
    distance *= 1.7 * this.scale;
    var cameraPosition = this.controls.target.clone().sub(this.camera.position).normalize().multiplyScalar(distance);
    this.controls.desiredCameraPosition = sceneCenter.clone().sub(cameraPosition);
    this.controls.maxDistance = distance * 10;
}

Thingiview.prototype.addModel = function (geometry) {  // 모델빛
    var obj, i;
    material = new THREE.MeshPhongMaterial({
        wrapAround: true,
        wrapRGB: new THREE.Vector3(0, 1, 1),
        color: 0xeeeeee,
        specular: 0xffffff,
        shading: THREE.SmoothShading,
        shininess: 150,
        fog: false,
        side: THREE.DoubleSide
    });
    mesh = new THREE.Mesh(geometry, material);
    mesh.geometry.computeBoundingBox();
    var dims = mesh.geometry.boundingBox.max.clone().sub(mesh.geometry.boundingBox.min);
    maxDim = Math.max(Math.max(dims.x, dims.y), dims.z);
    this.scale = 100 / maxDim;
    mesh.position.x = -(mesh.geometry.boundingBox.min.x + mesh.geometry.boundingBox.max.x) / 2 * this.scale;
    mesh.position.y = -(mesh.geometry.boundingBox.min.y + mesh.geometry.boundingBox.max.y) / 2 * this.scale;
    mesh.position.z = -mesh.geometry.boundingBox.min.z * this.scale;
    this.scene.add(mesh);
    this.models.push(mesh);
    for (var i = 0; i < this.models.length; i++)
        this.models[i].scale.x = this.models[i].scale.y = this.models[i].scale.z = this.scale;
    this.wirePlane.scale.x = this.wirePlane.scale.y = this.wirePlane.scale.z = this.scale;
    this.plane.scale.x = this.plane.scale.y = this.plane.scale.z = this.scale;
    this.centerCamera();

}

Thingiview.prototype.render = function () {
    if (this.visible == false) return;

    var now = Date.now();
    if (this.lastRenderTime == undefined)
        this.timeElapsed = 0;
    else
        this.timeElapsed = now - this.lastRenderTime;
    this.lastRenderTime = now;
    this.controls.dirty = false;
    this.controls.update(this.timeElapsed);
    this.reflectCamera.position.z = -this.camera.position.z;
    this.reflectCamera.position.y = this.camera.position.y;
    this.reflectCamera.position.x = this.camera.position.x;
    this.scene.traverse(function (object) {
        if (object.name == "plane" || object.name == "planewire")
            object.visible = false;
        if (object.name == "skybox")
            object.visible = true;
    });
    this.reflectCamera.updateCubeMap(this.renderer, this.scene);
    this.scene.traverse(function (object) {
        if (object.name == "plane" || object.name == "planewire")
            object.visible = true;
        if (object.name == "skybox")
            object.visible = false;
    });
    this.renderer.render(this.scene, this.camera);
    //console.log(this.camera);

    //this.getRendererData = this.renderer.domElement.toDataURL();
}


////볼륨 추가
//Thingiview.prototype.compute_vol = function (geo){
//    var x1, x2, x3, y1, y2, y3, z1, z2, z3, i;
//    var len = geo.faces.length;
//    var totalVolume = 0;

//    for (i = 0; i < len; i++) {
//        x1 = geo.vertices[geo.faces[i].a].x;
//        y1 = geo.vertices[geo.faces[i].a].y;
//        z1 = geo.vertices[geo.faces[i].a].z;
//        x2 = geo.vertices[geo.faces[i].b].x;
//        y2 = geo.vertices[geo.faces[i].b].y;
//        z2 = geo.vertices[geo.faces[i].b].z;
//        x3 = geo.vertices[geo.faces[i].c].x;
//        y3 = geo.vertices[geo.faces[i].c].y;
//        z3 = geo.vertices[geo.faces[i].c].z;

//        totalVolume +=
//            (-x3 * y2 * z1 +
//            x2 * y3 * z1 +
//            x3 * y1 * z2 -
//            x1 * y3 * z2 -
//            x2 * y1 * z3 +
//            x1 * y2 * z3) / 6;
//    }

//    volume = Math.abs(totalVolume);
//}