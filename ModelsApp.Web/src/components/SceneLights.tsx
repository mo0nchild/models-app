import React from 'react'

export interface SceneLightsProps {
    radius: number
    height: number
    intensity: number
    debug?: boolean
} 
export default function SceneLights(props: SceneLightsProps): React.JSX.Element {
    const { radius, height } = props;
    return (
    <group>
        {([
            [radius, height, -radius],
            [radius, height, radius],
            [-radius, height, radius],
            [-radius, height, -radius]
        ] as [number, number, number][]).map(item => {
            return (
            <group>
                <pointLight position={item} intensity={props.intensity} />
                { props.debug ? 
                <mesh position={item}>
                    <sphereGeometry args={[0.2]}/>
                    <meshPhongMaterial color="#fff" opacity={0.7} transparent />
                </mesh> : null}
            </group>
            )
        })
        }
    </group> 
    );
}