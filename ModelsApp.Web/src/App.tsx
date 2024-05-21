/* eslint-disable @typescript-eslint/ban-types */
import React from 'react'
import Model, { ModelLoadedProps } from './components/Model';
import './App.css'
import { CameraControls } from '@react-three/drei';
import { Canvas } from '@react-three/fiber';
import { v4 as uuidv4 } from 'uuid'
import { CameraInfo, CameraInfoHandler } from './components/CameraInfo';
import { Slider } from '@mui/material';
import SceneLights from './components/SceneLights';

export interface AppState {
	lightIntensity: number;
	sideIntensity: number;
	sideRadius: number;
	sideHeight: number;
	url: string | null
}
class App extends React.Component<{}, AppState> {
	private renderer = React.createRef<HTMLCanvasElement>();
	private imageRef = React.createRef<HTMLImageElement>();
	private cameraRef = React.createRef<CameraControls>();
	public constructor(props: {}) {
		super(props)
		this.state = { 
			url: null, 
			lightIntensity: 1, 
			sideIntensity: 1, 
			sideHeight: 5,
			sideRadius: 50
		}
	}
	private readonly getModelFromServer = async (name: string): Promise<Blob> => {
		const url = `http://localhost:8080/Models/getObject?name=${name}`;
		const result = await fetch(url, { method: 'GET' })
		return result.blob();
	}
	public override componentDidMount(): void {
		(async() => {
			const result = await this.getModelFromServer('chair.glb');
			console.log(`model size: ${result.size * Math.pow(10, -6)} mb`)
			this.setState({ url: URL.createObjectURL(result) })
		})();
	}
	private saveImageHandler = () => {
		if(this.state.url == null) return;
		const base64 = this.renderer.current?.toDataURL() as string;
		this.imageRef.current!.src = base64;
		
		window.fetch(`${base64}`)
		.then(value => value.blob())
		.then(value => {
			console.log(value)
			const data = new FormData();
			const uuid = uuidv4();
			console.log(uuid)
			data.append('file', new File([value], `${uuid}.png`));
			window.fetch('http://localhost:8080/Models/saveImage', { 
				method: 'POST',
				body: data
			})
			.then(value => value.json())
			.then(value => console.log(value))
			.catch(error => console.log(error));
		})
	}
	private modelLoadedHandler = ({info, size, vertices}: ModelLoadedProps) => {
		this.cameraRef.current?.setLookAt(size.x, size.y, size.z, 0, size.y / 2, 0, true)
		console.log(size)
		console.log(`vertices: ${vertices}`)
		this.setState({
			sideHeight: size.y / 2,
			sideRadius: size.z > size.x ? size.z : size.x
		})
	}
	private cameraInfoRef = React.createRef<CameraInfoHandler>();
	public override render(): React.ReactNode {
		const content = this.state.url != null 
			? 	<Model url={this.state.url} onLoaded={this.modelLoadedHandler}/>
			: 	<mesh position={[0, 0, -2]}>
					<sphereGeometry args={[2]} />
					<meshStandardMaterial color={0xff0000} />
				</mesh> 
		const { sideHeight, sideRadius } = this.state
		return (
			<div style={{width: '100%', height: '100%'}}>
				<Canvas camera={{ fov: 70, position: [1, 1, 1]}} 
						style={{ background: "#32a852", borderRadius: '10px' }} 
						ref={this.renderer} gl={{ preserveDrawingBuffer: true }}>
					<CameraControls makeDefault ref={this.cameraRef} truckSpeed={0.2} 
						maxPolarAngle={Math.PI * 0.6} minPolarAngle={Math.PI * 0.1}/>
					<ambientLight intensity={0.1} />
					{content}
					<axesHelper/>
					<CameraInfo ref={this.cameraInfoRef}/>
					<hemisphereLight intensity={this.state.lightIntensity / 10}/>
					<SceneLights height={sideHeight} intensity={this.state.sideIntensity}  
						radius={sideRadius} debug/>
				</Canvas>
				<button onClick={() => {
					const camera = this.cameraInfoRef.current?.getCamera();
					console.log({
						position: camera?.position,
						rotation: camera?.rotation
					})
				}}>Камера</button>
				<Slider value={this.state.lightIntensity} max={100} min={0} 
						onChange={(_, value) => {
					this.setState({lightIntensity: value as number})
				}} />
				<Slider value={this.state.sideIntensity} max={100} min={0} 
						onChange={(_, value) => {
					this.setState({sideIntensity: value as number})
				}} />
				<Slider value={sideRadius} max={50} min={1} 
						onChange={(_, value) => {
					this.setState({sideRadius: value as number})
				}} />
				<Slider value={sideHeight} max={10} min={1} 
						onChange={(_, value) => {
					this.setState({sideHeight: value as number})
				}} />

				<button onClick={this.saveImageHandler}>Сохранить изображение</button>
				<img ref={this.imageRef}/>
			</div>
		);
	}
}
export default App;

