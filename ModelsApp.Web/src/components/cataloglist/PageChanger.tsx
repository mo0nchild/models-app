import { ArrowBack, ArrowForward } from '@mui/icons-material';
import { Pagination, PaginationItem } from '@mui/material';
import React from 'react';
import ReactPaginate from 'react-paginate';

export interface PageChangerProps {
	pageCount: number;
	onPageChange: (page: number) => void
}
export default function PageChanger(props: PageChangerProps): React.JSX.Element {
	const { pageCount, onPageChange } = props
    return (
		<Pagination count={pageCount} shape="rounded" variant='outlined' renderItem={(item) => (
			<PaginationItem slots={{ previous: ArrowBack, next: ArrowForward }} 
				{...item} style={{color: '#FFF', backgroundColor: '#444'}} />
			)} boundaryCount={1} onChange={(_, page) => onPageChange(page)}
		/>
	)
}