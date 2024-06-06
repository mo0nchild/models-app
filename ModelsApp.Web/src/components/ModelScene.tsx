import { OrbitControls, PerspectiveCamera, useGLTF } from '@react-three/drei';
import { Canvas, useFrame, useThree } from '@react-three/fiber';
import React from 'react';
import * as THREE from 'three';
import * as THREESTD from 'three-stdlib';

export type ModelLoadedProps = {
    readonly info: THREE.WebGLInfo,
    readonly vertices: number,
    readonly triangles: number,
    readonly animations: string[] | null,
    readonly size: THREE.Vector3
}
export interface ModelObjectProps {
    url: string, onLoaded: (info: ModelLoadedProps) => void,
    currentAnimation: string | null
}
function ModelObject(props: ModelObjectProps): React.JSX.Element {
    const { url, onLoaded, currentAnimation } = props;
    const { gl } = useThree();
    const model = useGLTF<string>(url);
    if (currentAnimation != null) {

    }
    const [vertices, setVertices] = React.useState<number>(0);
    const [size, setSize] = React.useState<THREE.Vector3>(new THREE.Vector3());
    React.useEffect(() => {
        let boundingBox = new THREE.Box3().setFromObject(model.scene.children[0]);
        let vertices = 0, triangles = 0;
        for(const name in model.nodes) {
            const node = model.nodes[name];
            if(node instanceof THREE.SkinnedMesh || node instanceof THREE.Mesh) {
                vertices += (node.geometry.attributes.normal.count as number)
            }
        }
        model.scene.traverse(node => {
            if ((node as THREE.Mesh).isMesh) {
                const geometry = (node as THREE.Mesh).geometry as THREE.BufferGeometry;
                if (geometry.index) triangles += geometry.index.count / 3;
                else if (geometry.attributes.position) {
                    triangles += geometry.attributes.position.count / 3;
                }
            }
        })
        onLoaded({
            size: boundingBox.getSize(new THREE.Vector3()),
            info: gl.info,
            triangles: triangles,
            vertices: vertices,
            animations: model.animations.map(item => item.name)
        })
    }, [])
    return <primitive object={model.scene}/>
}
interface CameraInfoHandler { getCamera: () => THREE.Camera }
const CameraInfo = React.forwardRef<CameraInfoHandler, {}>(({}, ref) => {
    const { camera } = useThree();
    const cameraRef = React.useRef(camera);
    React.useImperativeHandle(ref, () => ({
        getCamera: () => {
            cameraRef.current.rotateY(0);
            cameraRef.current.updateProjectionMatrix()
            return cameraRef.current
        },
    }))
    cameraRef.current = camera;
    return null;
})
interface ControllerHandler { 
    setPosition: (target: THREE.Vector3) => void;
    getPosition: () => THREE.Vector3;

    setTarget: (vector: THREE.Vector3) => void;
    getTarget: () => THREE.Vector3
}
interface ControllerProps {
    readonly rotateSpeed: number,
    readonly maxDistance: number, 
    readonly minDistance: number
}
const CameraController = React.forwardRef<ControllerHandler, ControllerProps>((props, ref) => {
    const { rotateSpeed, maxDistance, minDistance } = props;
    const { camera, gl } = useThree();
    const controlsRef = React.useRef<THREESTD.OrbitControls>(null!);
    React.useImperativeHandle(ref, () => ({
        setTarget: ({x, y, z}) => {
            controlsRef.current.target.set(x, y, z);
            controlsRef.current.update();
        },
        getTarget: () => controlsRef.current.target,
        setPosition: ({x, y, z}) => {
            camera.position.set(x, y, z)
            controlsRef.current.update();
        },
        getPosition: () => camera.position
    }))
    return <OrbitControls ref={controlsRef} rotateSpeed={rotateSpeed} 
        args={[camera, gl.domElement]} 
        maxDistance={maxDistance} minDistance={minDistance}/>;
}) 
export interface AboutModel {
    readonly memorySize: number,
    readonly verticesCount: number,
    readonly trianglesCount: number,
    readonly animations: string[] | null
}
export interface SceneInfo extends AboutModel {
    readonly cameraSettings: { position: THREE.Vector3, target: THREE.Vector3 },
    readonly sceneImage: Blob
}
export interface ModelSceneHandler { getInfo: () => Promise<SceneInfo> }
export interface ModelSceneProps {
    readonly modelData: Blob,
    readonly sceneColor: string,
    readonly cameraSettings?: { position: THREE.Vector3, target: THREE.Vector3 },
    readonly lightIntensity: number,
    readonly currentAnimation: string | null
    onSceneLoaded?: (info: AboutModel) => void
}
export const ModelScene = React.forwardRef<ModelSceneHandler, ModelSceneProps> ((props, ref) => {
    const { modelData, cameraSettings, lightIntensity, sceneColor, currentAnimation } = props

    const controllerRef = React.useRef<ControllerHandler>(null);
    const rendererRef = React.useRef<HTMLCanvasElement>(null);

    const [ vertices, setVertices ] = React.useState<number>(0);
    const [ triangles, setTriangles ] = React.useState<number>(0);
    const [ animations, setAnimations ] = React.useState<string[] | null>(null);

    React.useImperativeHandle(ref, () => ({
        getInfo: async () => {
            const base64 = rendererRef.current?.toDataURL() as string;
            const imageData = await (window.fetch(base64).then(item => item.blob()))
            return {
                memorySize: (modelData.size * Math.pow(10, -6)),
                verticesCount: vertices, trianglesCount: triangles,
                cameraSettings: {
                    position: controllerRef.current!.getPosition(),
                    target: controllerRef.current!.getTarget()
                },
                sceneImage: imageData,
                animations: animations
            }
        }
    }), [vertices, triangles, modelData])
    const modelLoadedHandler = ({info, size, vertices, triangles, animations}: ModelLoadedProps) => {
        if (cameraSettings == undefined) {
            controllerRef.current?.setPosition(new THREE.Vector3(size.x, size.y, size.z));
            controllerRef.current?.setTarget(new THREE.Vector3(0, size.y / 2, 0));
        }
        else {
            controllerRef.current?.setPosition(cameraSettings.position);
            controllerRef.current?.setTarget(cameraSettings.target);
        }
        props.onSceneLoaded?.({
            memorySize: (modelData.size * Math.pow(10, -6)), 
            verticesCount: vertices,
            trianglesCount: triangles,
            animations: animations == null || animations.length <= 0 ? null : animations
        })
        console.log(animations)
        setAnimations(animations)
        setTriangles(triangles)
        setVertices(vertices)
    }
    const content = <ModelObject url={URL.createObjectURL(modelData)} onLoaded={modelLoadedHandler}
        currentAnimation={currentAnimation}/>
    return (
    <div style={{height: '400px', width: '100%', position: 'relative'}}>
        <Canvas ref={rendererRef} gl={{ preserveDrawingBuffer: true }}
                style={{borderRadius: '10px' }}>
            <PerspectiveCamera fov={60} near={0.01} far={1000}/>
            <CameraInfo/>
            <CameraController ref={controllerRef} maxDistance={1000} minDistance={0} rotateSpeed={1}/>
            <ambientLight intensity={1} />
            {content}
            <axesHelper position={[0, 0.1, 0]}/>
            <gridHelper scale={2} />
            <hemisphereLight intensity={lightIntensity}/>
            <color attach="background" args={[sceneColor]} />
        </Canvas>
    </div>
    );
})