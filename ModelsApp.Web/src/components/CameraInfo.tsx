import { useThree } from '@react-three/fiber';
import React from 'react';
import { Camera } from 'three';

export interface CameraInfoHandler {
    getCamera: () => Camera
}
export const CameraInfo = React.forwardRef<CameraInfoHandler, {}>(({}, ref) => {
    const { camera } = useThree();
    const cameraRef = React.useRef(camera);
    React.useImperativeHandle(ref, () => ({
        getCamera: () => cameraRef.current
    }))
    cameraRef.current = camera;
    return null;
}) 