import { ErrorInfo, ErrorInfoHandler } from "@components/ErrorInfo";
import ModelInfo, { ModelCreateInfo } from "@components/createmodel/ModelInfo";
import ModelLoading from "@components/createmodel/ModelLoading";
import SceneSettings from "@components/createmodel/SceneSettings";
import { Step, StepLabel, Stepper } from "@mui/material";
import { useApiAccessor, useCheckUser } from "@services/ApiAccess";
import { validationFormFactory } from "@services/ValidationField";
import React from "react";
import { Button, Col, Container, Row } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

export default function CreateModel(): React.JSX.Element {
    const errorRef = React.useRef<ErrorInfoHandler>(null);
    const navigator = useNavigate();
    const { checkUser } = useCheckUser();
    const { accessor } = useApiAccessor();   
    const [ step, setStep ] = React.useState<number>(0);

    const [ name, setName ] = React.useState<string | undefined>();
    const [ description, setDescription ] = React.useState<string | undefined>();
    const [ category, setCategory ] = React.useState<string | undefined>();
    const [ model, setModel ] = React.useState<Blob | undefined>()

    const [loadingShow, setLoadingShow] = React.useState<boolean>(false);
    const [loadedMessage, setLoadedMessage] = React.useState<string | null>(null);
    const modelInfoRef = React.useRef<ModelInfo>(null);
    const sceneSettingsRef = React.useRef<SceneSettings>(null);
    const closeError = () => {
        errorRef.current?.closeError();
        window.scrollTo(0, 0)
    }
    const steps = React.useMemo(() => (
    [ 
        { 
            name: 'Информация',
            element: <ModelInfo ref={modelInfoRef} accessor={accessor} name={name} 
                description={description} category={category}/> 
        },  
        { 
            name: 'Настройка сцены', 
            element: <SceneSettings ref={sceneSettingsRef} modelData={model}/>
        } 
    ]
    ), [model, name, description, category])
    const setError = (message: string) => {
        errorRef.current?.setError(message);
        window.scrollTo(0, 0)
    }
    React.useEffect(() => {
        const checkAsync = async () => {
            if(!(await checkUser())) return;
        }
        checkAsync().catch(error => console.log(error));
    }, [])
    const sendModelInfo = () => {
        setLoadingShow(true);
        (async () => {
            const sceneInfo = await sceneSettingsRef.current!.getSettings()
            const formData = new FormData();
            formData.append('name', name!);
            formData.append('description', description!);
            formData.append('categoryName', category!);
            formData.append('image', new File([sceneInfo.sceneImage], 'image.png'))

            formData.append('vertices', sceneInfo.verticesCount.toString())
            formData.append('triangles', sceneInfo.trianglesCount.toString())
            formData.append('memorySize', sceneInfo.memorySize.toFixed(2))
            formData.append('lightIntensity', sceneInfo.lightIntensity.toFixed(2))

            formData.append('file', new File([model!], 'model.glb'));
            formData.append('sceneColor', sceneInfo.sceneColor);
            const { position, target } = sceneInfo.cameraSettings
            formData.append('cameraX', position.x.toFixed(2));
            formData.append('cameraY', position.y.toFixed(2));
            formData.append('cameraZ', position.z.toFixed(2));
            formData.append('targetX', target.x.toFixed(2));
            formData.append('targetY', target.y.toFixed(2));
            formData.append('targetZ', target.z.toFixed(2));
            const response = await accessor({
                url: 'http://localhost:8080/modelsapp/models/add', 
                method: 'POST', body: formData
            })
            console.log(response)
        })()
        .then(() => setLoadedMessage('Модель успешно загружена'))
        .catch(error => {
            console.log(error)
            setLoadedMessage('Не удалось загрузить модель')
        })
    }
    const nextHandler = React.useCallback(() => {
        switch(step) {
            case 0: {
                const modelInfo = modelInfoRef.current?.getState();
                if (modelInfo == null || modelInfo.file == undefined) {
                    setError('Неообходимо загрузить файл')
                    return;
                }
                const { name, description, category } = modelInfo
                const formData = new FormData();
                formData.append('name', name == undefined ? '' : name);
                formData.append('description', description == undefined ? '' : description);

                const validation = validator(formData);
                if (validation != null) {
                    setError(validation)
                    return;
                }
                setName(name);
                setDescription(description)
                setCategory(category);
                setModel(modelInfo.file);
            } break;
            case 1: sendModelInfo(); break;
        }
        if(step < 1) setStep(step + 1);
        closeError()
    }, [step])
    const backHandler = React.useCallback(() => {
        if(step >= 1) setStep(step - 1);
        closeError()
    }, [step])
    const onContinueHandler = React.useCallback(() => window.location.pathname = '/', [])
    const renderCreatePanel = (): React.JSX.Element => (
        <div className='p-4 p-md-5 panel-info'>
            <h1 className='fs-4 mb-4'>Добавление модели:</h1>
            <Stepper activeStep={step} alternativeLabel>
                {steps.map((item) => (
                    <Step key={item.name}>
                        <StepLabel>
                            <span style={{ color: '#FFF' }}>{item.name}</span>
                        </StepLabel>
                    </Step>
                ))}
            </Stepper>
            <div style={{ margin: '50px 0px' }}>
                {steps[step].element}
            </div>
            <Container fluid={'md'}>
                <Row className='justify-content-center gy-2'>
                    <Col xs={6} md={6} lg={5}>
                        <Button onClick={backHandler} style={{
                            visibility: step == 0 ? 'hidden' : 'visible',
                            width: '100%',
                        }}>Назад</Button>
                    </Col>
                    <Col xs={6} md={6} lg={5}>
                        <Button onClick={nextHandler} style={{ width: '100%' }}>Далее</Button>
                    </Col>
                </Row>
            </Container>
            <ModelLoading message={loadedMessage} show={loadingShow} 
                onContinue={onContinueHandler} />
        </div>
    )
    return (
    <div>
        <Container fluid={'md'}>
            <Row className='justify-content-center'>
                <Col sm='12' md='10' lg='10' xl='8'>
                    <ErrorInfo ref={errorRef}/>
                </Col>
            </Row>
            <Row className='justify-content-center'>
                <Col sm='12' md='10' lg='10' xl='8'>
                    {renderCreatePanel()}
                </Col>
            </Row>
        </Container>
    </div>
    )
}
const validator = validationFormFactory([
    { 
        name: 'name',
        rule: (value) =>  value.length >= 5, 
        message: 'Длина названия больше 5 символов',
    },
    { 
        name: 'description',
        rule: (value) =>  value.length >= 10, 
        message: 'Длина описания больше 10 символов',  
    }
])
