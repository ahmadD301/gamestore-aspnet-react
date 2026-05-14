import { useQuery }
  from "@tanstack/react-query";

import { getGenres }
  from "../../api/genresApi";

export function useGenres() {
  return useQuery({
    queryKey: ["genres"],

    queryFn: getGenres,

    staleTime: 1000 * 60 * 10,
  });
}