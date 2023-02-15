import React, { FC, useEffect, useState } from 'react';
import axios, { AxiosError } from 'axios';
import { Message } from '../../Types/Timeline';
import { IsLoading } from '../../Types/Global';
import { Box } from '@mui/material';
import TimeLineMessage from './TimeLineMessage';

interface props {
	user: string | null;
}

const TimeLineContainer: FC<props> = ({ user }) => {
	const [timeline, setTimeline] = useState([] as Message[]);
	const [isLoading, setIsLoading] = useState({ isLoading: true, error: null } as IsLoading);

	useEffect(() => {
		const getTimeline = async () => {
			try {
				const response = await axios.get(`${process.env.REACT_APP_API_URL}/public`, {
					headers: {
						'access-control-allow-origin': '*',
					},
				});
				setTimeline(response.data);
				setIsLoading({ isLoading: false, error: null });
			} catch (e: any) {
				if (e instanceof AxiosError) {
					console.log(e);
					setIsLoading({ isLoading: false, error: e.message });
				} else {
					console.log(e);
					setIsLoading({ isLoading: false, error: 'Something went wrong!' });
				}
			}
		};
		getTimeline();
	}, []);

	if (isLoading.isLoading && isLoading.error === null) {
		return <div>Loading...</div>;
	} else if (isLoading.error !== null) {
		return <div>Error: {isLoading.error}</div>;
	}

	const style = {
		container: {
			display: 'flex',
			flexDirection: 'column',
			alignItems: 'center',
			justifyContent: 'center',
			width: '70%',
			margin: 'auto',
		},
	};

	return (
		<Box sx={style.container}>
			{timeline.map((message) => {
				return <TimeLineMessage message={message} key={message.id} />;
			})}
		</Box>
	);
};

export default TimeLineContainer;
