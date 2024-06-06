import { CircularProgress } from '@mui/material';
import React from 'react';
import { Modal, Button } from 'react-bootstrap';

export interface ModelLoadingProps {
    readonly show: boolean;
    readonly message: string | null;
    onContinue: () => void;
}
export default function ModelLoading(props: ModelLoadingProps) : React.JSX.Element {
    const { show, message, onContinue } = props
    return (
    <Modal show={show} centered>
            <Modal.Header>
                <Modal.Title>Загрузка модели</Modal.Title>
            </Modal.Header>
            <Modal.Body style={{ padding: '20px' }}>
            { 
                message == null ? 
                <div style={{ display: 'flex', flexFlow: 'column', alignItems: 'center' }}>
                    <CircularProgress />
                    <p>Загрузка...</p>
                </div>
                : <div style={{ display: 'flex', flexFlow: 'column', alignItems: 'center' }}>
                    <p>{message}</p>
                    <Button onClick={() => onContinue()}>Продолжить</Button>
                </div>
            }
            </Modal.Body>
            <Modal.Footer style={{ padding: '10px 20px 20px' }}>
            </Modal.Footer>
        </Modal>
    )
}