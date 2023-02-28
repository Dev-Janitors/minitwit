import { Box } from "@mui/material";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Header from "./Components/Global/Header/Header";
import TimeLineContainer from "./Components/PublicTimeline/TimeLineContainer";

function App() {
  const style = {
    container: {
      display: "flex",
      flexDirection: "column",
      alignItems: "center",
      justifyContent: "center",
    },
  };
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/"
          element={
            <Box sx={style.container}>
              <Header />
              <TimeLineContainer />
            </Box>
          }
        >
          <Route path="user/:username" element={<TimeLineContainer />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
