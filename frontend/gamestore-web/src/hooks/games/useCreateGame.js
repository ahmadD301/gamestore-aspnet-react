import { useMutation,
  useQueryClient }
from "@tanstack/react-query";

import { createGame }
from "../../api/gamesApi";

export function useCreateGame() {
  const queryClient =
    useQueryClient();

  return useMutation({
    mutationFn: createGame,

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["games"],
      });
    },
  });
}