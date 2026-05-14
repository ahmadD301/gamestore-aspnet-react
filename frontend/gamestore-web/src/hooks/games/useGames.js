import { useQuery }
  from "@tanstack/react-query";

import { getGames }
  from "../../api/gamesApi";

export function useGames(query) {
  return useQuery({
    queryKey: ["games", query],

    queryFn: () => getGames(query),

    staleTime: 1000 * 60,

    retry: 1,
  });
}