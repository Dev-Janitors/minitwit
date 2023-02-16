import React, { FC, Fragment, useEffect, useState } from 'react';
import axios, { AxiosError } from 'axios';
import { Message } from '../../Types/Timeline';
import { IsLoading } from '../../Types/Global';
import TimeLineMessage from './TimeLineMessage';
import List from '@mui/material/List';
import Divider from '@mui/material/Divider';
import { Typography } from '@mui/material';

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

	if (timeline.length === 0) {
		return <Typography variant="h5">No messages</Typography>;
	}

	return (
		<List sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
			{timeline.map((message, i) => {
				return (
					<Fragment key={i}>
						<TimeLineMessage message={message} />
						{/* {i !== timeline.length - 1 && <Divider variant="inset" component="li" />} */}
					</Fragment>
				);
			})}
		</List>
	);
};

export default TimeLineContainer;
