import { Routes, Route } from "react-router-dom";
import TimeLineContainer from "./Components/PublicTimeline/TimeLineContainer";
import {useAuth0} from "@auth0/auth0-react"
import AuthenticationGuard from "./Components/Authentication/AuthenticationGuard";
import MainPage from "./Components/Pages/MainPage";
import { PageLoader } from "./Components/Authentication/PageLoader";
import NotFound from "./Components/Pages/NotFound";

function App() {
  const {isLoading} = useAuth0();

  if(isLoading){
    return <div className="page-layout">
      <PageLoader/>
    </div>
  }

  return (
      <Routes>
        <Route
          path="/"
          element={
            <MainPage/>
          }
        >
        </Route>
        <Route path="user/:username" element={
            <AuthenticationGuard component={TimeLineContainer} />
          } />
        <Route path="*" element={<NotFound/>}/>
      </Routes>
  );
}

export default App;
