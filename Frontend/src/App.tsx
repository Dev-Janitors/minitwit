import { Box } from '@mui/material';
import Header from './Components/Global/Header/Header';
import TimeLineContainer from './Components/PublicTimeline/TimeLineContainer';

function App() {
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
			<Header />
			<TimeLineContainer user={null} />
		</Box>
	);
}

export default App;
