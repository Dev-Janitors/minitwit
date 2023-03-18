import { Routes, Route } from 'react-router-dom';
import NotFound from './Components/Pages/NotFound';
import TimelinePage from './Components/Pages/TimelinePage';
import SnackbarContextProvider from './Components/SnackBar/SnackbarContextProvider';
import Snackbar from './Components/SnackBar/Snackbar';
import MyTimeline from './Components/Pages/MyTimeline';

function App() {
	// const { isLoading } = useAuth0();

	// if (isLoading) {
	// 	return (
	// 		<div className="page-layout">
	// 			<PageLoader />
	// 		</div>
	// 	);
	// }

	return (
		<SnackbarContextProvider>
			<Routes>
				<Route path="/" element={<TimelinePage />}></Route>
				<Route path="user/:username" element={<TimelinePage />} />
				<Route path="*" element={<NotFound />} />
				<Route path=":username/my-timeline" element={<MyTimeline/>}/>
			</Routes>
			<Snackbar />
		</SnackbarContextProvider>
	);
}

export default App;
