import { Routes, Route } from 'react-router-dom';
import NotFound from './Components/Pages/NotFound';
import TimelinePage from './Components/Pages/PublicTimelinePage';
import SnackbarContextProvider from './Components/SnackBar/SnackbarContextProvider';
import Snackbar from './Components/SnackBar/Snackbar';
import MyTimelinePage from './Components/Pages/MyTimelinePage';
import UserTimelinePage from './Components/Pages/UserTimelinePage';

function App() {

	return (
		<SnackbarContextProvider>
			<Routes>
				<Route path="/" element={<TimelinePage />}/>
				<Route path="user/:username" element={<UserTimelinePage />} />
				<Route path="*" element={<NotFound />} />
				<Route path=":username/my-timeline" element={<MyTimelinePage/>}/>
			</Routes>
			<Snackbar />
		</SnackbarContextProvider>
	);
}

export default App;
