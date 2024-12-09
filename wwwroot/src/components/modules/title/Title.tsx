
type Props = {
  title: string;
  className?: string;
};

const Title = (props: Props) => {
  return (
    <div className={`${props.className ? props.className : ""} mb-6 flex flex-row-reverse items-center justify-end gap-3 !text-2xl font-bold`}>
      <h5 className="mt-1 max-sm:!text-xl">{props.title}</h5>
     </div>
  );
};

export default Title;
