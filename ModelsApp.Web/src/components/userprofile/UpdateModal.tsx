import React from 'react';
import Avatar from 'react-avatar-edit';
import { Button, Form, Modal } from 'react-bootstrap';

export type UpdateInfo = {
    name: string;
    biography: string,
    image: Blob | undefined
}
export interface UpdateModalProps {
    currentInfo? : {
        name: string,
        biography: string,
        image: string
    }
    show: boolean;
    onAccept?: (info: UpdateInfo) => void
    onClose: () => void,
}
export default function UpdateModal({show, currentInfo, onClose, onAccept}: UpdateModalProps)
    : React.JSX.Element {
    const [avatar, setAvatar] = React.useState<string>();
    const nameRef = React.useRef<HTMLInputElement>(null);
    const biographyRef = React.useRef<HTMLTextAreaElement>(null);

    const modalCloseHandler = React.useCallback(() => {
        setAvatar(undefined)
        onClose.call(undefined);
    }, [])
    const modalAcceptHandler = React.useCallback(async () => {
        const name = nameRef.current?.value!;
        const biography = biographyRef.current?.value!;
        let image: Blob | undefined = undefined;
        if(avatar != undefined) {
            const convert = await window.fetch(avatar);
            image = await convert.blob();
        }
        onAccept?.call(undefined, {name, biography, image});
        modalCloseHandler()
    }, [avatar])
    return (
    <Modal show={show} centered onHide={modalCloseHandler}>
        <Modal.Header closeButton>
            <Modal.Title>Изменение информации</Modal.Title>
        </Modal.Header>
        <Modal.Body style={{padding: '0px 10px'}}>
            <div style={{display: 'flex', flexFlow: 'column', alignItems: 'center'}}>
                <div className="mb-3" style={{width: '250px', height: '250px', overflow: 'hidden'}}>
                    <Avatar width={250} height={250} imageHeight={250} onCrop={data => setAvatar(data)}
                        src={currentInfo == undefined ? undefined : currentInfo.image} 
                        onClose={() => setAvatar(undefined)} cropRadius={250 / 2}/>
                </div>
                <Form.Group className="mb-3 px-3" style={{width: '100%'}}>
                    <Form.Label>Имя пользователя:</Form.Label>
                    <Form.Control defaultValue={currentInfo == undefined ? '' : currentInfo.name} 
                        ref={nameRef} type="email" placeholder="Ваше имя"
                        style={infoFieldStyle} />
                </Form.Group>
                <Form.Group className="mb-3 px-3" style={{width: '100%'}}>
                    <Form.Label>Информация о себе:</Form.Label>
                    <Form.Control defaultValue={currentInfo == undefined ? '' : currentInfo.biography} 
                        ref={biographyRef} as="textarea" rows={3} placeholder='Информация о вас'
                        style={infoFieldStyle}/>
                </Form.Group>
            </div>
        </Modal.Body>
        <Modal.Footer style={{padding: '10px 20px 20px'}}>
            <Button className='modal-btn' onClick={modalCloseHandler}>
                Закрыть
            </Button>
            <Button className='modal-btn' onClick={modalAcceptHandler}>
                Сохранить
            </Button>
        </Modal.Footer>
    </Modal>
    );
}
const infoFieldStyle: React.CSSProperties = {
    backgroundColor: '#323232',
    color: '#FFF',
    resize: 'none'
}