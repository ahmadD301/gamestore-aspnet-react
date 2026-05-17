import { useMutation,
  useQueryClient }
from "@tanstack/react-query";

import { updateGame }
from "../../api/gamesApi";

export function useUpdateGame() {
  const queryClient =
    useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      payload,
    }) =>
      updateGame(id, payload),

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["games"],
      });
    },
  });
}