import React, { FC } from 'react';
import { Box } from '@mui/material';
import Header from '../Global/Header/Header';
import TimeLineContainer from '../PublicTimeline/TimeLineContainer';

const TimelinePage: FC = () => {
	const style = {
		container: {
			display: 'flex',
			flexDirection: 'column',
			alignItems: 'center',
			justifyContent: 'center',
		},
	};
	return (
		<Box sx={style.container}>
			<Header title="Public Timeline"/>
			<TimeLineContainer />
		</Box>
	);
};

export default TimelinePage;
