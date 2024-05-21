/* eslint-disable @typescript-eslint/ban-types */
import { useGLTF } from '@react-three/drei';
import { useThree } from '@react-three/fiber';
import React, { useEffect } from 'react';
import { Box3, Mesh, SkinnedMesh, Vector3, WebGLInfo } from 'three';

export type ModelLoadedProps = {
    info: WebGLInfo,
    vertices: number,
    size: Vector3
}
export interface ModelProps {
    readonly url: string;
    onLoaded: (info: ModelLoadedProps) => void;
}
const Model: React.FC<ModelProps> = ({url, onLoaded}) => {
    const model = useGLTF<string>(url);
    // const action = mixer.clipAction(model.animations[5]);
    // action.play();
    const { gl } = useThree();
    useEffect(() => {    
        const boundingBox = new Box3().setFromObject(model.scene.children[0]);
        let vertices = 0;
        for(const name in model.nodes) {
            const node = model.nodes[name];
            if(node instanceof SkinnedMesh || node instanceof Mesh) {
                vertices += (node.geometry.attributes.normal.count as number)
            }
        }
        onLoaded({
            size: boundingBox.getSize(new Vector3()),
            info: gl.info,
            vertices: vertices,
        })
    }, [])
    return (
        <primitive object={model.scene} />
    )
}
export default Model;